export const manifest = (() => {
function __memo(fn) {
	let value;
	return () => value ??= (value = fn());
}

return {
	appDir: "_app",
	appPath: "_app",
	assets: new Set(["favicon.ico","robots.txt","staticwebapp.config.json"]),
	mimeTypes: {".txt":"text/plain",".json":"application/json"},
	_: {
		client: {start:"_app/immutable/entry/start.CioscS6T.js",app:"_app/immutable/entry/app.BtkcsCti.js",imports:["_app/immutable/entry/start.CioscS6T.js","_app/immutable/chunks/L5XTwUF4.js","_app/immutable/chunks/Bnso1Vyc.js","_app/immutable/chunks/3c0xzto3.js","_app/immutable/chunks/DZrG6KVW.js","_app/immutable/entry/app.BtkcsCti.js","_app/immutable/chunks/DXb0ZbaM.js","_app/immutable/chunks/Bnso1Vyc.js","_app/immutable/chunks/3c0xzto3.js","_app/immutable/chunks/CT0T0Gak.js"],stylesheets:[],fonts:[],uses_env_dynamic_public:false},
		nodes: [
			__memo(() => import('./nodes/0.js')),
			__memo(() => import('./nodes/1.js')),
			__memo(() => import('./nodes/2.js')),
			__memo(() => import('./nodes/3.js')),
			__memo(() => import('./nodes/4.js')),
			__memo(() => import('./nodes/5.js')),
			__memo(() => import('./nodes/6.js'))
		],
		remotes: {
			
		},
		routes: [
			{
				id: "/",
				pattern: /^\/$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 2 },
				endpoint: null
			},
			{
				id: "/calendar",
				pattern: /^\/calendar\/?$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 3 },
				endpoint: null
			},
			{
				id: "/event",
				pattern: /^\/event\/?$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 4 },
				endpoint: null
			},
			{
				id: "/list",
				pattern: /^\/list\/?$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 5 },
				endpoint: null
			},
			{
				id: "/map",
				pattern: /^\/map\/?$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 6 },
				endpoint: null
			}
		],
		prerendered_routes: new Set([]),
		matchers: async () => {
			
			return {  };
		},
		server_assets: {}
	}
}
})();
