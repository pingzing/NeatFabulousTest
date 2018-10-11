namespace FabulousTest

module Detail =
    open Fabulous.Core
    open Fabulous.DynamicViews
    open Xamarin.Forms

    type Model = {
        ImageUrl : string
    }

    type Msg = 
    | GoBack

    let init () =
        { ImageUrl = ""}, Cmd.none

    let view (model: Model) dispatch =
        View.ContentPage(
            title = "Detail",            
            content = View.Grid(
                rowdefs = ["auto"; "*"],
                children = [
                    View.Button( 
                        text = "Back", 
                        horizontalOptions = LayoutOptions.Start,
                        command = (fun () -> dispatch GoBack)
                    ).GridRow(0)
                    View.Image(model.ImageUrl).GridRow(1)
                ]
            )
        )