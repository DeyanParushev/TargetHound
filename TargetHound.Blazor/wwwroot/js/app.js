import * as THREE from "./three.js";
import * as OrbitControlls from "./OrbitControlls.js";
import * as DragControlls from "./DragControlls.js";
import * as VertexNormalHelpers from "./VertexNormalHelpers.js";

export function RenderProject(project) {
    RenderScene();
}

export function RenderBorehole(borehole) {
    //ShowBorehole(borehole);
    console.log(borehole.name);
}

function RenderScene() {
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

function ShowBorehole() {
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth * 0.8, window.innerHeight * 0.9);
    document.body.appendChild(renderer.domElement);

    const cubeGeometry = new THREE.BoxGeometry(0.2, 0.2, 0.2);
    const cubeMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
    const cube = new THREE.Mesh(cubeGeometry, cubeMaterial);
    scene.add(cube);

    AddArrows(scene)

    //if (borehole !== undefined) {
    //    //DrawBorehole(scene, borehole);
    //    console.log(borehole);
    //}

    camera.position.z = 5;

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

function AddPlane(scene) {
    var geo = new THREE.PlaneBufferGeometry(1000, 1000, 8, 8);
    var mat = new THREE.MeshBasicMaterial({ color: 0xffffff, opacity: 0.9, side: THREE.DoubleSide });
    var plane = new THREE.Mesh(geo, mat);
    scene.add(plane);
}

function DrawBorehole(scene, borehole) {
    const collar = new THREE.BoxGeometry(0.2, 0.2, 0.2);
    const cubeMaterial = new THREE.MeshBasicMaterial({ color: 0x375A7F });
    const cube = new THREE.Mesh(collar, cubeMaterial);
    scene.add(cube);

    borehole.surveyPoints.forEach(x => AddPoint(scene, borehole.collar, x));
}

function AddPoint(scene, collar, point) {
    var pointGeometry = new THREE.SphereGeometry(1, 16, 16, 6, 6, 6, 6);
    var pointMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
    var sphere = new THREE.Mesh(pointGeometry, pointMaterial);

    sphere.position.x = 0 + (collar.easting - point.easting);
    sphere.position.y = 0 + (collar.norhing - point.norhing);
    sphere.position.z = 0 + (collar.elevation - point.elevation);

    scene.add(sphere);
}
