'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:EnvironmentsCtrl
 * @description
 * # EnvironmentsCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
  .controller('EnvironmentsCtrl', ($scope, $resource, $http, $window, $log, $modal) => {
    $scope.environments = [];
    $scope.createFormIsCollapsed = true;

    var Environments = $resource('http://localhost:2403/environments/:id.json', null, {
                          'update': { method: 'PUT' }
                       });
    //var Environments = $resource('scripts/mocks/environments-mock.js', {});
    $scope.environments = Environments.query();

    $scope.iconsList = [];

    $scope.getIcons = () => {
      if ($scope.iconsList.length > 0) {
        return $scope.iconsList;
      }

      return $http.get('data/icons.js').success((icons) => {
        $scope.iconsList = icons;
        $log.log('Got icons. Length: ' + icons.length);
      });
    };

    $scope.selectIcon = (model) => {
      $scope.env.icon = 'fa-' + model.id;
    };

    $scope.open = function (env) {
      var modalInstance = $modal.open({
          templateUrl: 'editModal.html',
          controller: ModalInstanceCtrl,
          size: 'lg',
          resolve: {
            environment: () => { return angular.copy(env); }
          }
      });

      modalInstance.result.then((updatedEnvironment) => {
        Environments.update({id: updatedEnvironment.shortName}, updatedEnvironment, (res) => {
            $log.log('Updated environment');
            $scope.environments = Environments.query();
        });
      });
    };

    $scope.sortableOptions = {
      update: (e, ui) => {
        $log.log('Update',
         ui.item.sortable.index, ui.item.sortable.dropindex
        );
      },
      stop: (e, ui) => {
        $log.log('Stop',
         ui.item.sortable.index, ui.item.sortable.dropindex
        );

        for (var i = 0; i < $scope.environments.length; i++) {
          $log.log(i, $scope.environments[i]);
        }

      }
    };

    $scope.submit = () => {
      $log.log('Saving', $scope.env);

      new Environments($scope.env)
        .$save()
          .then(env => {
            $scope.environments = Environments.query();
            $scope.createFormIsCollapsed = true;
          })
          .catch(e => { $log.error('Saving failed', e); });
    };

    $scope.remove = (env) => {
      if (!$window.confirm('Do you want to delete ' + env.shortName + '?')) {
        return;
      }

      $log.log('Removing', env);

      Environments.remove({id: env.shortName}, (envRes) => {
        $scope.environments = Environments.query();
      });
    };
  });

var ModalInstanceCtrl = function ($scope, $http, $log, $modalInstance, environment) {
  $scope.env = environment;
  $scope.originalEnv = angular.copy(environment);

  $scope.iconsList = [];

  $scope.getIcons = () => {
    if ($scope.iconsList.length > 0) {
      return $scope.iconsList;
    }

    return $http.get('data/icons.js').success((icons) => {
      $scope.iconsList = icons;
      $log.log('Got icons. Length: ' + icons.length);
    });
  };

  $scope.selectIcon = (model) => {
    $scope.env.icon = 'fa-' + model.id;
  };


  $scope.ok = () => {
    $modalInstance.close($scope.env);
  };

  $scope.reset = () => {
    $scope.env = angular.copy($scope.originalEnv);
  };

  $scope.cancel = () => {
    $modalInstance.dismiss('cancel');
  };
};
