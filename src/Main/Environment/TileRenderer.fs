module Environment.TileSelector

open Environment
open Model
open Microsoft.Xna.Framework
open MonoGame.Extended
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

let rand = new System.Random()

let SelectTile (env:Environment) x y =
    match Map.tryFind (x,y) env with
        | Some x -> env, x
        | None ->
            let tile:ClimateTile = if rand.Next(0,2) = 1 then Debug else Hot Sand
            Map.add (x,y) tile env, tile //TODO: Separate tile rendering and generation

let tilesize = 10

let RenderTiles (graphics:GraphicsDeviceManager) (cam:Camera2D) (env:Environment) content (sprites:SpriteBatch) =
    let width = graphics.PreferredBackBufferWidth
    let height = graphics.PreferredBackBufferHeight

    let tileheight = width/(tilesize*2)
    let tilewidth = width/tilesize

    let camx, camy = cam.Position.X, cam.Position.Y
    let tilex, tiley = int camx/tilewidth, int camy/tileheight
    let off = new Vector2(camx%float32 tilewidth, camy%float32 (tilewidth/2))

    let rows = height/tileheight
    let row = [-6 .. tilesize*2+6]
    let tiles = [-6 .. rows+6] |> List.collect (fun y -> row |> List.map (fun x -> (tileheight*y)+(int x)%2*(tileheight/2), x*tilewidth/2))

    tiles |> List.fold (fun env (y,x) ->
        let env, tile = SelectTile env ((x/tilewidth)-tilex) ((y/tileheight)-tiley)
        let tex = Map.find tile content

        let vec = new Vector2(camx+off.X+float32 x, camy+off.Y+(y-tilewidth/2 |> float32))
        let destrect = new Rectangle(int vec.X, int vec.Y, tilewidth, tilewidth)
        sprites.Draw(tex, destrect, Color.White)

        env) env