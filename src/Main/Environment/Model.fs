module Environment.Model

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Graphics

type HotTile = Sand | StonePath | StoneWall
type MediumTile = Grass | GrassPath | DirtWall
type ColdTile = Snow | SnowPath

type ClimateTile = Debug | Hot of HotTile | Medium of MediumTile | Cold of ColdTile
type TileSet = Map<ClimateTile, Texture2D>

type Environment = Map<int*int, ClimateTile>

let tiles = Debug::([Sand; (*StonePath; StoneWall*)] |> List.map Hot)//@([Grass; GrassPath; DirtWall] |> List.map Medium)@([Snow; SnowPath] |> List.map Cold)

let getTile (content:ContentManager) tile =
    let str = match tile with | Hot x -> x.ToString() | Medium x -> x.ToString() | Cold x -> x.ToString() | Debug -> "Debug"
    content.Load<Texture2D>(str |> sprintf "Tiles\\%s")

let defaultenv:Environment = Map.empty