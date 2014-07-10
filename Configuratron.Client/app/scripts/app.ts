/// <reference path='../typings/_all.d.ts' />
'use strict';

/**
 * @ngdoc overview
 * @name ctronApp
 * @description
 * # ctronApp
 *
 * Main module of the application.
 */
angular
	.module('ctronApp', [
		'ngAnimate',
		'ngCookies',
		'ngResource',
		'ngRoute',
		'ngSanitize',
		'ui.bootstrap',
		'ui.sortable',
		'xeditable'
	])
	.config(($routeProvider) => {
		$routeProvider
			.when('/', {
				templateUrl: 'views/main.html',
				controller: 'MainCtrl'
			})
			.when('/about', {
				templateUrl: 'views/about.html',
				controller: 'AboutCtrl'
			})
			.when('/environments', {
				templateUrl: 'views/environments.html',
				controller: 'EnvironmentsCtrl'
			})
		  .when('/applications', {
				templateUrl: 'views/applications.html',
				controller: 'ApplicationsCtrl'
			})
		  .when('/applications/:appName', {
				templateUrl: 'views/application-details.html',
				controller: 'ApplicationDetailsCtrl'
			})
			.otherwise({
				redirectTo: '/'
			});
	})
	.run(function(editableOptions, editableThemes) {
		editableOptions.theme = 'bs3';
		editableThemes.bs3.inputClass = 'input-sm';
		editableThemes.bs3.buttonsClass = 'btn-sm';
	});