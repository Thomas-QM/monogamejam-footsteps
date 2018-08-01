module Environment.TileSelector

open GameEnvironment
open Model
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Comora

let rand = new System.Random()

let SelectTile ({Tiles=env}:GameEnvironment) x y =
    match Map.tryFind (x,y) env with
        | Some (_, x) -> env, x
        | None ->
            let (climate, tile) = if rand.Next(0,2) = 1 then (Hot,Water) else (Hot,Sand)
            Map.add (x,y) (climate, tile) env, tile //TODO: Separate tile rendering and generation, make a tile array to store tile positions which update on update as well

let tilesize = 10

let UpdateTiles (graphics:GraphicsDeviceManager) (cam:ICamera) (env:GameEnvironment) =
    let width = cam.Width |> int
    let height = cam.Height |> int

    let tileheight = width/(tilesize*2)
    let tilewidth = width/tilesize

    let extratiles = 5

    let campos = cam.Position-Vector2(cam.Width/float32 2, cam.Height/float32 2)
    let camx, camy = campos.X-float32 (extratiles*tilewidth), campos.Y-float32 (extratiles*tileheight)
    let tilex, tiley = (camx |> int)/tilewidth, (camy |> int)/tileheight
    let off = Vector2(camx%float32 tilewidth, camy%float32 tileheight)

    let rows = height/tileheight
    let row = [0 .. (tilesize*2)+(extratiles*2)]
    let tiles =
        [0 .. rows+(extratiles*2)]
        |> List.collect (fun y -> row |> List.map (fun x -> (tileheight*y)+(x%2) * (tileheight/2),
                                                            x*tilewidth/2))

    tiles |> List.fold (fun env (y,x) ->
        let newtiles, tile = SelectTile env ((x/tilewidth)+tilex) ((y/tileheight)+tiley)

        let vec = Vector2(camx-off.X+float32 x, camy-off.Y+(y-tilewidth/2 |> float32))
        let collision = if collide |> List.contains tile then Block else NoCollision

        let rendertile = {Region=new Rectangle(int vec.X-1, int vec.Y-1, tilewidth+1, tilewidth+1);
                            Tile=tile; Collision=collision}

        {env with Tiles=newtiles; RenderTiles=rendertile::env.RenderTiles}) {env with RenderTiles=[]}

let RenderTiles (graphics:GraphicsDeviceManager) ({RenderTiles=tiles}:GameEnvironment) content (sprites:SpriteBatch) =
    tiles |> List.iter (fun {Region=pos; Tile=tile;} ->
        let tex = Map.find tile content
        sprites.Draw(tex, pos, Color.White)
    )