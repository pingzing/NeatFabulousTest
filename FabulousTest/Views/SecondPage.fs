namespace FabulousTest.Views

module SecondPage = 
    open Fabulous.DynamicViews
    open FabulousTest.ModelType

    let modelType = ModelType.SecondPage

    type Model = {
        Title : string
    }

    let initModel = { Title="I am the second page." } 

    let view (model: Model) = View.ContentPage(title = model.Title)