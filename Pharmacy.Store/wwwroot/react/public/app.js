window.globalConfig = {
  cahcheName: 'pharma-pwa-1',
  baseUrl: window.location.origin.indexOf('localhost:3000') >= 0 ? 'https://localhost:44328/' : 'https://pharma.hillavas.com/api/'
};

if ('serviceWorker' in navigator) {
  window.addEventListener('load', function () {
    var currentUrl = window.location;
    navigator.serviceWorker.register(currentUrl.origin + '/worker.js', { scope: '/' }).then(function (registration) {
      console.log('Worker registration successful', registration.scope);
    }, function (err) {
      console.log('Worker registration failed', err);
    }).catch(function (err) {
      console.log(err);
    });
  });
} else {
  console.log('Service Worker is not supported by browser.');
}