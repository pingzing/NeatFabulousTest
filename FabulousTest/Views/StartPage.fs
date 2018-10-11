namespace FabulousTest.Views

module StartPage = 
    open Fabulous.DynamicViews    
    open FabulousTest.ModelType
    open Fabulous.Core
    open Xamarin.Forms

    let modelType = ModelType.StartPage

    type Msg = 
        | Increment 
        | Decrement 
        | Reset
        | SetStep of int
        | TimerToggled of bool
        | TimedTick

    let timerCmd = 
        async { do! Async.Sleep 200
                return TimedTick }
        |> Cmd.ofAsyncMsg

    type Model = 
      { Count : int
        Step : int
        TimerOn: bool
        Title: string }

    let initModel = { Count = 0; Step = 1; TimerOn=false; Title="I am the start page!" } 

    let init () = initModel, Cmd.none

    let view (model: Model) dispatch = 
            View.ContentPage(
                content = View.StackLayout(padding = 20.0, verticalOptions = LayoutOptions.Center, children = [ 
                    View.Label(text = sprintf "%d" model.Count, horizontalOptions = LayoutOptions.Center, widthRequest=200.0, horizontalTextAlignment=TextAlignment.Center)
                    View.Button(text = "Increment", command = (fun () -> dispatch Increment), horizontalOptions = LayoutOptions.Center)
                    View.Button(text = "Decrement", command = (fun () -> dispatch Decrement), horizontalOptions = LayoutOptions.Center)
                    View.Label(text = "Timer", horizontalOptions = LayoutOptions.Center)
                    View.Switch(isToggled = model.TimerOn, toggled = (fun on -> dispatch (TimerToggled on.Value)), horizontalOptions = LayoutOptions.Center)
                    View.Slider(minimum = 0.0, maximum = 10.0, value = double model.Step, valueChanged = (fun args -> dispatch (SetStep (int (args.NewValue + 0.5)))), horizontalOptions = LayoutOptions.FillAndExpand)
                    View.Label(text = sprintf "Step size: %d" model.Step, horizontalOptions = LayoutOptions.Center) 
                    View.Button(text = "Reset", horizontalOptions = LayoutOptions.Center, command = (fun () -> dispatch Reset), canExecute = (model <> initModel))
                ]))

    let update msg model = 
        match msg with
        | Increment -> { model with Count = model.Count + model.Step }, Cmd.none
        | Decrement -> { model with Count = model.Count - model.Step }, Cmd.none
        | Reset -> init ()
        | SetStep n -> { model with Step = n }, Cmd.none
        | TimerToggled on -> { model with TimerOn = on }, (if on then timerCmd else Cmd.none)
        | TimedTick -> 
            if model.TimerOn then 
                { model with Count = model.Count + model.Step }, timerCmd
            else 
                model, Cmd.none