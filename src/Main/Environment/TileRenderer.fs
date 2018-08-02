module GameEnvironment.TileSelector

open GameEnvironment
open Model
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Comora
open Utility
open Utility.Coord

let rand = new System.Random()

let SelectTile ({Tiles=env}:GameEnvironment) x y =
    match Map.tryFind (x,y) env with
        | Some (_, x) -> env, x
        | None ->
            let (climate, tile) = if rand.Next(0,2) = 1 then (Hot,Water) else (Hot,Sand)
            Map.add (x,y) (climate, tile) env, tile //TODO: Separate tile rendering and generation, make a tile array to store tile positions which update on update as well

let tilesize = 10
let extratiles = 5

let UpdateTiles (graphics:GraphicsDeviceManager) (cam:ICamera) (env:GameEnvironment) =
    let width = cam.Width |> int
    let height = cam.Height |> int

    let tilewidth = width/tilesize
    let tileheight = height/(tilesize*2)

    let extrasize = Vector2(extratiles*(tileheight/2) |> float32, extratiles*(tileheight/2) |> float32)
    let relative = (cam.Position - Vector2(float32 0, cam.Height)) - extrasize//*Vector2(float32 1, float32 2))
    let campos = (CamPos cam) - extrasize

    let tilex, tiley = (campos.X |> int)/tilewidth, (campos.Y |> int)/tilewidth
    let off = Vector2(campos.X%float32 tilewidth, campos.Y%float32 tilewidth)

    let rows = height/tileheight
    let row = [0 .. (tilesize)*2+extratiles]
    let tiles =
        [0 .. rows+extratiles]
        |> List.collect (fun y -> row |> List.map (fun x -> y*tilewidth/2, x*tilewidth/2))

    tiles |> List.fold (fun env (y,x) ->
        let newtiles, tile = SelectTile env ((x/tilewidth)+tilex) ((y/tilewidth)+tiley)

        let pos = Vector2(float32 x-off.X, float32 y-off.Y)
        let collision = if collide |> List.contains tile then Block else NoCollision

        let rendertile = {Relative=relative; Position=pos; Width=tilewidth; Tile=tile; Collision=collision}

        {env with Tiles=newtiles; RenderTiles=rendertile::env.RenderTiles}) {env with RenderTiles=[]}

let RenderTiles (graphics:GraphicsDeviceManager) (cam:ICamera) ({RenderTiles=tiles}:GameEnvironment) content (sprites:SpriteBatch) =
    tiles |> List.iter (fun {Position=pos; Relative=relative; Width=width; Tile=tile;} ->
        let tex = Map.find tile content

        let pos = pos |> CartesianToIsometric |> (fun x -> x+relative)
        sprites.Draw(tex, new Rectangle(int pos.X, int pos.Y, width, width), Color.White)
    )