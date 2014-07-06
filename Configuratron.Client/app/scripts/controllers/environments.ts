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
