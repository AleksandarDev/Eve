/// <reference path="../jquery.d.ts" />
/// <reference path="../knockout.d.ts" />

$(document).ready(() => {
	$("#accountName").hover(() => {
		$("#accountMenu").stop(true, true).slideDown("slow");
	});
	$("#accountBar").hover(() => { }, () => {
		$("#accountMenu").stop(true, true).slideUp("slow");
	});
});
