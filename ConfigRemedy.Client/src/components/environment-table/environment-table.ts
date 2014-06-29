/// <amd-dependency path="text!./environment-table.html" />
import ko = require("knockout");
export var template: string = require("text!./environment-table.html");

export class viewModel {

    public environments = ko.observableArray();

    constructor (params: any) {
		$.getJSON("http://localhost:8090/environments/", this.environments);
    }

    public del(el) {
    	debugger
    	$.ajax({
			url: "http://localhost:8090/environments/" + el.name,
			type: "DELETE"
		});
    }

    public dispose() {
        // This runs when the component is torn down. Put here any logic necessary to clean up,
        // for example cancelling setTimeouts or disposing Knockout subscriptions/computeds.        
    }
}
