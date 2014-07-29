/// <reference path='../../typings/_all.d.ts' />
/**
 * Created by evgenyb on 26.07.2014.
 */
'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:DiagnosticsCtrl
 * @description
 * # DiagnosticsCtrl
 * Controller of the ctronApp
 */


angular.module('ctronApp')
    .controller('DiagnosticsCtrl', ($scope, DiagnosticsService) => {
        $scope.diagnostics = DiagnosticsService.getDiagnostics();
    });

