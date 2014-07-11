'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:ApplicationsCtrl
 * @description
 * # ApplicationsCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
 .controller('ApplicationsCtrl', ($scope, $resource, $log, $window) => {
   $scope.applications = [];

   var Applications = $resource('http://localhost:2403/applications/:id.json', null, {
                                  'update': { method: 'PUT' }
                                });

   $scope.applications = Applications.query();

   $scope.edit = (app) => {
      $window.alert('Not done');
   };

   $scope.remove = (app) => {
      if (!$window.confirm('Are you sure you want to delete ' + app.name + '?')) {
        return;
      }

      Applications.remove({id: app.name}, () => {
        $scope.applications = Applications.query();
      });
   };


});
