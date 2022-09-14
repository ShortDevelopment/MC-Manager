/// <reference path="../../OverlayContract.d.ts" />

import { BlockRaycastOptions, Player, world, Block, EntityHealthComponent, Entity, Dimension } from "mojang-minecraft";
import { http, HttpHeader, HttpRequest, HttpRequestMethod } from "mojang-net";

{
    const req = new HttpRequest("http://localhost:3000/test");
    req.body = JSON.stringify({
        foo: "bar"
    });
    req.method = HttpRequestMethod.POST;
    req.headers = [
        new HttpHeader("Content-Type", "application/json"),
    ];
    http.request(req);
}

const players = Array.from(world.getPlayers());

/** @type {WorldInfo | null} */
let worldInfo = null;

/** @type {PlayerInfo | null} */
let playerInfo = null;

const raycastMaxDistance = 50;

/** @type{BlockRaycastOptions} */
const blockRaycast = {
    includeLiquidBlocks: false,
    includePassableBlocks: false,
    maxDistance: raycastMaxDistance
};

/** @type{BlockRaycastOptions} */
const liquidRaycast = {
    includeLiquidBlocks: true,
    includePassableBlocks: false,
    maxDistance: raycastMaxDistance
};

/** @type{BlockRaycastOptions} */
const passableRaycast = {
    includeLiquidBlocks: false,
    includePassableBlocks: true,
    maxDistance: raycastMaxDistance
};

const overworld = world.getDimension("minecraft:overworld");

world.events.tick.subscribe(async (e) => {
    overworld.runCommand("tell @a tick");

    worldInfo = {
        tick: e,
        weather: null
    };

    const player = players[0];
    if (player) {
        playerInfo = {
            id: player.id,
            health: /** @type{EntityHealthComponent} */(player.getComponent(EntityHealthComponent.id)),
            name: player.name,
            location: player.location,
            rotation: player.rotation,
            dimension: player.dimension.id,
            biome: undefined,
            lightLevel: undefined,
            molang: undefined,
            targetBlock: ProcessTargetBlock(player.getBlockFromViewVector(blockRaycast)),
            targetLiquid: ProcessTargetBlock(player.getBlockFromViewVector(liquidRaycast)),
            targetPassable: ProcessTargetBlock(player.getBlockFromViewVector(passableRaycast)),
            targetEntities: ProcessEntities(player.getEntitiesFromViewVector({ maxDistance: raycastMaxDistance }))
        };
    } else {
        playerInfo = null;
    }

    const req = new HttpRequest("http://localhost:3000/updateData");
    req.body = JSON.stringify({
        world: worldInfo,
        player: playerInfo
    });
    req.method = HttpRequestMethod.POST;
    req.headers = [
        new HttpHeader("Content-Type", "application/json"),
    ];
    await http.request(req);
});

/**
 * 
 * @param {Block} block
 * @returns {BlockInfo}
 */
function ProcessTargetBlock(block) {
    return {
        id: block.id,
        location: block.location,
        properties: block.permutation.getAllProperties()
    };
}

/**
 * 
 * @param {Entity} entity
 * @returns {EntityInfo}
 */
function ProcessEntity(entity) {
    return {
        id: entity.id,
        location: entity.location,
        rotation: entity.rotation,
        health: /** @type{EntityHealthComponent} */(player.getComponent(EntityHealthComponent.id)),
        molang: null
    };
}

/**
 * 
 * @param {Entity[]} entities
 * @returns {EntityInfo[]}
 */
function ProcessEntities(entities) {
    /** @type {EntityInfo[]} */
    const result = [];
    for (const entity of entities) {
        result.push(ProcessEntity(entity));
    }
    return result;
}