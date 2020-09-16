var CACHE_NAME = 'pharma-pwa-1';
var urlsToCache = [
  './',
  './logo.png',
  './home-left-bg.jpg',
  './home-right-bg.jpg',
  './offline-drug.png',
  './pharmacy.png',
  "./manifest.json",
  "https://unpkg.com/leaflet@1.6.0/dist/leaflet.css",
  "https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick.min.css",
  "https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick-theme.min.css",
];
const baseUrl = 'https://pharma.hillavas.com';
const apiUrl = 'https://pharma.hillavas.com/api/';

// const baseUrl = 'localhost:3000';
// const apiUrl = 'https://localhost:44328/';

// Install a service worker
self.addEventListener('install', e => {
  // Perform install steps
  e.waitUntil(
    caches.open(CACHE_NAME)
      .then(function (cache) {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
});

self.addEventListener('fetch', e => {
  if (e.request.url.indexOf(baseUrl) > -1 || urlsToCache.indexOf(e.request.url) >= 0) {
    //for shell
    e.respondWith(caches.match(e.request).then(function (cRep) {
      var fRep = null;
      try {
        fRep = fetch(e.request)
          .then(function (response) {
            return caches.open(CACHE_NAME).then(function (cache) {
              cache.put(e.request, response.clone());
              return response;
            });
          });
      }
      catch (e) { }

      return cRep || fRep;
    })
    );
  }
});

// Cache and return requests
// self.addEventListener('fetch', e => {

//   if (e.request.url.indexOf(apiUrl) === 0)//for api 
//   {
//     e.respondWith(
//       fetch(e.request)
//         .then(function (response) {
//           return caches.open(CACHE_NAME).then(function (cache) {
//             cache.put(e.request.url, response.clone());
//             console.log('[ServiceWorker] Fetched&Cached Data');
//             return response;
//           });
//         })
//         .catch(function (err) { })
//     );
//   } else if (e.request.url.indexOf(baseUrl) === 0 || urlsToCache.indexOf(e.request.url) >= 0) {
//     //for shell
//     e.respondWith(caches.match(e.request.url).then(function (cRep) {
//       var fRep = fetch(e.request)
//         .then(function (response) {
//           return caches.open(CACHE_NAME).then(function (cache) {
//             cache.put(e.request.url, response.clone());
//             return response;
//           });
//         });
//       return fRep || cRep;
//     })
//     );
//   }
// });

// Update a service worker
self.addEventListener('activate', event => {
  var cacheWhitelist = [CACHE_NAME];
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.map(cacheName => {
          if (cacheWhitelist.indexOf(cacheName) === -1) {
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
});