/// <reference path='../../typings/_all.d.ts' />
'use strict';

/**
 * @ngdoc function
 * @name ctronApp.services:DiagnosticsService
 * @description
 * # DiagnosticsService
 * service of the ctronApp
 */
angular.module('ctronApp')
    .service('DiagnosticsService', function ($resource, $log, configuration) {

        var Diagnostics = $resource(configuration.ApiBaseUrl + 'diagnostics.json');

        this.getDiagnostics = function () {
            return Diagnostics.get();
        };

        return this;
    });

