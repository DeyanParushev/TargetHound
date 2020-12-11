import * as THREE from "./three.js";

export function RenderProject(project) {
    const scene = new THREE.Scene();
    console.log(project.Id + 'From JS@@');
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth * 0.8, window.innerHeight);
    document.body.appendChild(renderer.domElement);
    
    const geometry = new THREE.BoxGeometry();
    const material = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
    const cube = new THREE.Mesh(geometry, material);
    scene.add(cube);

    camera.position.z = 5;

    var container = document.getElementsByClassName("3dContainer")[0];
    while (container.lastElementChild) {
        container.removeChild(container.lastElementChild);
    }

    container.appendChild(renderer.domElement);
    animate();
    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
    }
}

export function RenderBorehole(borehole) {
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth * 0.8, window.innerHeight);
    document.body.appendChild(renderer.domElement);

    const geometry = new THREE.BoxGeometry();
    const material = new THREE.MeshBasicMaterial({ color: 0x00AA00 });
    const cube = new THREE.Mesh(geometry, material);
    console.log(borehole.Id + 'From JS!!');
    scene.add(cube);

    camera.position.z = 5;

    var container = document.getElementsByClassName("3dContainer")[0];
    while (container.lastElementChild) {
        container.removeChild(container.lastElementChild);
    }

    container.appendChild(renderer.domElement);
    animate();
    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
    }
}