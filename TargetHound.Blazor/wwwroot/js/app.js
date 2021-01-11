import * as THREE from "./three.js";
import * as OrbitControlls from "./OrbitControlls.js";
import * as DragControlls from "./DragControlls.js";
import * as VertexNormalHelpers from "./VertexNormalHelpers.js";

export function RenderProject() {
    RenderScene();
}

export function RenderBorehole(borehole) {
    RenderScene(borehole);
}

function RenderScene(borehole) {
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth * 0.8, window.innerHeight * 0.9);
    document.body.appendChild(renderer.domElement);

    AddArrows(scene)
    camera.position.z = 5;

    const cubeGeometry = new THREE.BoxGeometry(1.5, 1.5, 1.5);
    const cubeMaterial = new THREE.MeshBasicMaterial({ color: 0x375A7F });
    const cube = new THREE.Mesh(cubeGeometry, cubeMaterial);
    scene.add(cube);

    var mouse = new THREE.Vector2();

    var container = document.getElementsByClassName("3dContainer")[0];
    while (container.lastElementChild) {
        container.removeChild(container.lastElementChild);
    }

    var orbitControlls = new OrbitControlls.OrbitControls(camera, container);

    var objects = [camera];
    var dragControls = new DragControlls.DragControls(objects, camera, container);
    dragControls.addEventListener('dragstart', function () { orbitControlls.enabled = false; });
    dragControls.addEventListener('dragend', function () { orbitControlls.enabled = true; });

    if (borehole !== null) {
        DrawBorehole(scene, borehole);
        cube.position.set(borehole.collar.easting, borehole.collar.northing, borehole.collar.elevation);
        camera.position.set(borehole.collar.easting + 5, borehole.collar.northing + 5, borehole.collar.elevation + 5);
        orbitControlls.target.set(borehole.collar.easting, borehole.collar.northing, borehole.collar.elevation);
    }

    container.appendChild(renderer.domElement);
    window.addEventListener('resize', (event) => {
        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(window.innerWidth * 0.8, window.innerHeight * 0.9);
    });
    container.addEventListener('mousemove', OnMouseMove, false);

    animate();

    function animate() {
        cube.rotation.x += 0.01;
        cube.rotation.y += 0.01;
        cube.rotation.z += 0.01;
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
        orbitControlls.update();
        render();
    }

    function OnMouseMove(event) {
        event.preventDefault();

        mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
        mouse.y = - (event.clientY / window.innerHeight) * 2 + 1;
    }

    function render() {
        var raycaster = new THREE.Raycaster();
        raycaster.setFromCamera(mouse, camera);

        var meshPoints = [];
        scene.children.forEach(x => {
            if (x.type === 'Mesh') {
                if (x.geometry.type === 'SphereGeometry') {
                    meshPoints.push(x);
                }
            }
        });
        var intersects = raycaster.intersectObjects(scene.children);

        if (intersects.length > 0) {

            //if (INTERSECTED != intersects[0].object) {

            //    if (INTERSECTED) INTERSECTED.material.emissive.setHex(INTERSECTED.currentHex);

            //    INTERSECTED = intersects[0].object;
            //    INTERSECTED.currentHex = INTERSECTED.material.emissive.getHex();
            //    INTERSECTED.material.emissive.setHex(0xff0000);
            //}
            var object = intersects[0];
            console.log(object.position);
            
        } else {

            //if (INTERSECTED) INTERSECTED.material.emissive.setHex(INTERSECTED.currentHex);

            //INTERSECTED = null;

            console.log('no intersection');
        }

        renderer.render(scene, camera);

    }
}

function DrawBorehole(scene, borehole) {
    DrawLine(scene, borehole.surveyPoints);
    AddPoints(scene, borehole.surveyPoints);
    DrawCollar(scene, borehole.collar);
    DrawTarget(scene, borehole.target);
}

function AddArrows(scene) {
    var horizontalY = new THREE.Vector3(0, 1, 0);
    horizontalY.normalize();
    var origin = new THREE.Vector3(0, 0, 0);
    var length = 1;
    var hex = 0x00ff00;
    var arrowHelper = new THREE.ArrowHelper(horizontalY, origin, length, hex);
    scene.add(arrowHelper);

    var horizontalX = new THREE.Vector3(1, 0, 0);
    horizontalX.normalize();
    hex = 0xff0000;
    var horizontalArrow = new THREE.ArrowHelper(horizontalX, origin, length, hex);
    scene.add(horizontalArrow);

    var vertical = new THREE.Vector3(0, 0, 1);
    vertical.normalize();
    hex = 0x0000ff;
    var horizontalArrow = new THREE.ArrowHelper(vertical, origin, length, hex);
    scene.add(horizontalArrow);
}

function DrawCollar(scene, collar) {
    var pointGeometry = new THREE.SphereGeometry(1, 32, 32, 6, 6, 6, 6);
    var pointMaterial = new THREE.MeshBasicMaterial({ color: 0xffffff });
    var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

    sphere.position.x = collar.easting;
    sphere.position.y = collar.northing;
    sphere.position.z = collar.elevation;

    scene.add(sphere);
}

function DrawTarget(scene, target) {
    var pointGeometry = new THREE.SphereGeometry(4, 32, 32, 6, 6, 6, 6);
    var pointMaterial = new THREE.MeshBasicMaterial({ color: 0x0f00ff });
    var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

    sphere.position.x = target.easting;
    sphere.position.y = target.northing;
    sphere.position.z = target.elevation;
    scene.add(sphere);
}

function AddPoints(scene, surveyPoints) {
    surveyPoints.forEach(x => {
        var pointGeometry = new THREE.SphereGeometry(1, 32, 32, 6, 6, 6, 6);
        var pointMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
        var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

        sphere.position.x = x.easting;
        sphere.position.y = x.northing;
        sphere.position.z = x.elevation;
        scene.add(sphere);
    });
}

function DrawLine(scene, surveyPoints) {
    var pointsCoordinates = [];
    surveyPoints.forEach(x => {
        var vectorPoint = new THREE.Vector3(x.easting, x.northing, x.elevation);
        pointsCoordinates.push(vectorPoint);
    });

    const curve = new THREE.CatmullRomCurve3(pointsCoordinates);
    const points = curve.getPoints(pointsCoordinates.length);
    const geometry = new THREE.BufferGeometry().setFromPoints(points);

    const material = new THREE.LineBasicMaterial({ color: 0xff0000 });
    const curveObject = new THREE.Line(geometry, material);
    scene.add(curveObject);
}


