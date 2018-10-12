namespace FabulousTest

module Gallery =
    open Fabulous.Core
    open Fabulous.DynamicViews
    open Xamarin.Forms
    
    type Model = {
        ImageUrls : string list
        SelectedImageIndex : int option
    }

    type Msg = 
    | ViewImage of string
    | SelectImage of int option

    let init () = 
        { SelectedImageIndex = None 
          ImageUrls = [
            "https://images.pexels.com/photos/67636/rose-blue-flower-rose-blooms-67636.jpeg"
            "https://www.w3schools.com/w3css/img_lights.jpg"
          ] }, 
        Cmd.none

    let update msg (model: Model) =
        match msg with 
        | SelectImage imageIndex ->             
            match imageIndex with 
            | None -> { model with SelectedImageIndex = imageIndex }, Cmd.none
            | Some newi -> match model.SelectedImageIndex with
                           | Some oldi -> 
                               if newi = oldi 
                               then model, List.item newi model.ImageUrls |> ViewImage |> Cmd.ofMsg 
                               else { model with SelectedImageIndex = Some newi }, Cmd.none
                           | None -> { model with SelectedImageIndex = Some newi }, Cmd.none
                                                            

    let view (model: Model) dispatch =
        View.ContentPage(
            title = "Gallery",
            content = View.ListView(
                selectedItem = model.SelectedImageIndex,
                itemTapped =  (fun idx -> dispatch(Some idx |> SelectImage)),
                items = [for img in model.ImageUrls do 
                            yield View.Image(source = img, 
                                margin = Thickness(5.0) )]
            )
        )
    

