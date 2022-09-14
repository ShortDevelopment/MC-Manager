/// <reference path="../HostContract.d.ts" />
/// <reference path="../OverlayContract.d.ts" />

import { createApp } from 'vue';

class Vector3 {
    /**
     * 
     * @param {number} x
     * @param {number} y
     * @param {number} z
     */
    constructor(x, y, z) {
        /** @type {number} */
        this.X = x;
        /** @type {number} */
        this.Y = y;
        /** @type {number} */
        this.Z = z;
    }
}

let vueRef = null;

createApp({
    data() {
        return {
            message: 'Hello Vue!',
            /** @type {OverlayDataMessage} */
            data: null
        }
    },
    methods: {
        /**
         * 
         * @param {IVector3} loc
         */
        formatLocation(loc) {
            return `${loc.x} / ${loc.y} / ${loc.z}`;
        },
        /**
         * 
         * @param {IVector2} rot
         */
        formatRotation(rot) {
            return `${rot.x} / ${rot.y}`;
        },
        updateData(data) {
            this.data = data;
        }
    },
    mounted() {
        vueRef = this;
    }
}).mount('#app');

window.chrome.webview.addEventListener('message', (e) => {
    vueRef.updateData(e);
});