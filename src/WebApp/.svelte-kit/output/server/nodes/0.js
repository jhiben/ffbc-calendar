

export const index = 0;
let component_cache;
export const component = async () => component_cache ??= (await import('../entries/pages/_layout.svelte.js')).default;
export const universal = {
  "prerender": false,
  "ssr": false
};
export const universal_id = "src/routes/+layout.ts";
export const imports = ["_app/immutable/nodes/0.DkaiKQEo.js","_app/immutable/chunks/3c0xzto3.js","_app/immutable/chunks/BnoHcUkP.js","_app/immutable/chunks/L5XTwUF4.js","_app/immutable/chunks/Bnso1Vyc.js","_app/immutable/chunks/DZrG6KVW.js","_app/immutable/chunks/CT0T0Gak.js"];
export const stylesheets = ["_app/immutable/assets/0.BAVb0Ww-.css"];
export const fonts = [];
