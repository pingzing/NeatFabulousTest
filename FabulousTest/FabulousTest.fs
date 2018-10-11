namespace FabulousTest

open Fabulous.Core
open Xamarin.Forms

module App = 

    type Page = Gallery | Detail

    type AppModel = {
        CurrentPage : Page
        GalleryModel : Gallery.Model
        DetailModel : Detail.Model
    }

    type Msg = 
    | GalleryMsg of Gallery.Msg
    | DetailMsg of Detail.Msg

    let init () =
        let galleryInitState, galleryInitCmd = Gallery.init ()
        let detailInitState, detailInitCmd = Detail.init ()
        let initModel = {
            GalleryModel = galleryInitState
            DetailModel = detailInitState
            CurrentPage = Gallery
        }

        initModel, Cmd.batch [Cmd.map GalleryMsg galleryInitCmd; Cmd.map DetailMsg detailInitCmd ]

    let update msg (model: AppModel) =
        match msg with
        | GalleryMsg (Gallery.Msg.ViewImage url) ->
            let nextDetailModel = { model.DetailModel with ImageUrl = url }            
            { model with DetailModel = nextDetailModel; CurrentPage = Detail }, Cmd.none
        | GalleryMsg galleryMsg ->
            let currentGalleryModel = model.GalleryModel
            let nextGalleryModel, nextGalleryCmd = Gallery.update galleryMsg currentGalleryModel
            let nextModel = { model with GalleryModel = nextGalleryModel }
            nextModel, Cmd.map GalleryMsg nextGalleryCmd
        | DetailMsg (Detail.Msg.GoBack) ->            
            { model with GalleryModel = model.GalleryModel; CurrentPage = Gallery}, Cmd.none

    let view (model: AppModel) dispatch =
       match model.CurrentPage with 
       | Gallery -> Gallery.view model.GalleryModel (GalleryMsg >> dispatch)
       | Detail -> Detail.view model.DetailModel (DetailMsg >> dispatch)

    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgram init update view

type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> Program.runWithDynamicView app

