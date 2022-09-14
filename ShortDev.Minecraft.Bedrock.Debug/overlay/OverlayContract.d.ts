/// <reference path="addon/node_modules/@types/mojang-minecraft/index.d.ts" />

interface IOverlayMessage {
    type: string;
}

interface OverlayDataMessage extends IOverlayMessage {
    type: 'data';
    player: PlayerInfo;
    game: GameInfo;
    world: WorldInfo;
}

interface EntityInfo {
    id: string;
    location: IVector3;
    rotation: IVector2;
    health: HealthInfo;
    molang: any;
}

interface HealthInfo {
    current: number;
    value: number;
}

interface BlockInfo {
    id: string;
    location: IVector3;
    properties: IBlockProperty[];
}

interface PlayerInfo extends EntityInfo {
    name: string;
    biome: string;
    dimension: string;
    lightLevel: number;
    targetBlock: BlockInfo?;
    targetLiquid: BlockInfo?;
    targetPassable: BlockInfo?;
    targetEntities: EntityInfo[]?;
}

interface GameInfo {
    name: string;
    version: string;
}

interface WorldInfo {
    tick: TickEvent;
    weather: any;
}

interface IVector3 {
    x: number;
    y: number;
    z: number;
}

interface IVector2 {
    x: number;
    y: number;
}