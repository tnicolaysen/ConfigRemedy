/// <amd-dependency path="text!./add-env-form.html" />
import ko = require("knockout");
import $ = require("jquery");

export var template: string = require("text!./add-env-form.html");

export class viewModel {

	public name = ko.observable("");
	
	public submit = () => {
		$.ajax({
			url: "http://localhost:8090/environments/",
			data: { name: this.name() },
			type: "POST"
		})
		.done((res) => {
			alert(res);
		});
	}

	constructor (params: any) {

	}

	public dispose() {
		// This runs when the component is torn down. Put here any logic necessary to clean up,
		// for example cancelling setTimeouts or disposing Knockout subscriptions/computeds.        
	}
}
