module GameEnvironment.Model

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Graphics

type Tile = Sand | Water | StonePath | StoneWall | Grass | GrassPath | DirtWall | Snow | SnowPath

type Climate = Debug | Hot | Medium | Cold
type ClimateTile = Climate*Tile
type TileSet = Map<Tile, Texture2D>
type Collision = Block | NoCollision

type RenderTile = {Region:Rectangle; Tile:Tile; Collision:Collision}
type GameEnvironment = {RenderTiles: RenderTile list; Tiles:Map<int*int, ClimateTile>}

let tiles = [Hot,Sand; Hot,Water;]//@([Grass; GrassPath; DirtWall] |> List.map Medium)@([Snow; SnowPath] |> List.map Cold)

let collide = [Water]

let getTile (content:ContentManager) tile =
    content.Load<Texture2D>(tile.ToString() |> sprintf "Tiles\\%s")

let defaultenv:GameEnvironment = {RenderTiles=[]; Tiles=Map.empty}