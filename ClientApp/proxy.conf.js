const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:58020';

const PROXY_CONFIG = [
  {
  //  context: [
  //    "/weatherforecast",
  //  ],
  //  proxyTimeout: 10000,
  //  target: target,
  //  secure: false,
  //  headers: {
  //    Connection: 'Keep-Alive'
  //  }
  //}
    context: [
      "yt/*",
    ],
    proxyTimeout: 10000,
    target: "https://youtube.com",
    secure: true,
    "changeOrigin":true    
  }
]

module.exports = PROXY_CONFIG;
