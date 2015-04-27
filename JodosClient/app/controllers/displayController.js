'use strict';
app.controller('displayController', ['$scope', '$location','$http', '$templateCache', 'authService','$rootScope' ,function ($scope, $location,$http ,$templateCache,authService,$rootScope) {


    $scope.searchtext = 'iphone';
    $scope.message ='';
    $scope.testFun = $rootScope.testRoot;
    $scope.testProduct = [];
    
   
}]);