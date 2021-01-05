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

    const cubeGeometry = new THREE.BoxGeometry(0.2, 0.2, 0.2);
    const cubeMaterial = new THREE.MeshBasicMaterial({ color: 0x375A7F });
    const cube = new THREE.Mesh(cubeGeometry, cubeMaterial);
    scene.add(cube);

    AddArrows(scene)
    camera.position.z = 5;

    if (borehole != null) {
        DrawBorehole(scene, borehole);
    }

    var container = document.getElementsByClassName("3dContainer")[0];
    while (container.lastElementChild) {
        container.removeChild(container.lastElementChild);
    }

    var orbitControlls = new OrbitControlls.OrbitControls(camera, container);

    var objects = [camera];
    var dragControls = new DragControlls.DragControls(objects, camera, container);
    dragControls.addEventListener('dragstart', function () { orbitControlls.enabled = false; });
    dragControls.addEventListener('dragend', function () { orbitControlls.enabled = true; });

    container.appendChild(renderer.domElement);
    animate();

    function animate() {
        cube.rotation.x += 0.01;
        cube.rotation.y += 0.01;
        cube.rotation.z += 0.01;
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
        orbitControlls.update();
    }
}

function DrawBorehole(scene, borehole) {
    var points = [];
    borehole.surveyPoints.forEach(x => ConvertCoordinate(points, borehole.collar, x));
    DrawLine(scene, points);
    DrawCollar(scene, borehole.collar);
    DrawTarget(scene, borehole.collar, borehole.target);
    AddPoints(scene, borehole.collar, points);
}

function ConvertCoordinate(collection, referencePoint, point) {
    var vectorPoint = new THREE.Vector3(
        referencePoint.easting - point.easting,
        referencePoint.northing - point.northing,
        referencePoint.elevation - point.elevation);

    collection.push(vectorPoint);
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
    var pointGeometry = new THREE.SphereGeometry(0.2, 16, 16, 6, 6, 6, 6);
    var pointMaterial = new THREE.MeshBasicMaterial({ color: 0xffff00 });
    var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

    sphere.position.x = collar.easting;
    sphere.position.y = collar.northing;
    sphere.position.z = collar.elevation;

    scene.add(sphere);
}

function DrawTarget(scene, collar, target) {
    var pointGeometry = new THREE.SphereGeometry(0.2, 16, 16, 6, 6, 6, 6);
    var pointMaterial = new THREE.MeshBasicMaterial({ color: 0x0000ff });
    var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

    sphere.position.x = 0 + (collar.easting - target.easting);
    sphere.position.y = 0 + (collar.northing - target.northing);
    sphere.position.z = 0 + (collar.elevation - target.elevation);
    scene.add(sphere);
}

function AddPoints(scene, collar, surveyPoints) {
    surveyPoints.forEach(x => {
        var pointGeometry = new THREE.SphereGeometry(5, 16, 16, 6, 6, 6, 6);
        var pointMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
        var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

        sphere.position.x = collar.easting - x.x;
        sphere.position.y = collar.northing - x.y;
        sphere.position.z = collar.elevation - x.z;
        scene.add(sphere);
    })
}

function DrawLine(scene, surveyPoints) {
    const curve = new THREE.CatmullRomCurve3(surveyPoints);
    console.log(surveyPoints.length);
    const points = curve.getPoints(surveyPoints.length);
    const geometry = new THREE.BufferGeometry().setFromPoints(points);

    const material = new THREE.LineBasicMaterial({ color: 0xff0000 });
    const curveObject = new THREE.Line(geometry, material);
    scene.add(curveObject);
}
