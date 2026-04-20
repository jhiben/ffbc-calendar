import { svelte } from '@sveltejs/vite-plugin-svelte';
import { defineConfig } from 'vitest/config';
import { fileURLToPath } from 'node:url';
import path from 'node:path';

const __dirname = path.dirname(fileURLToPath(import.meta.url));

export default defineConfig({
	plugins: [svelte({ compilerOptions: { runes: true } })],
	resolve: {
		conditions: ['browser']
	},
	test: {
		include: ['src/**/*.test.ts'],
		environment: 'jsdom',
		setupFiles: ['src/test-setup.ts'],
		alias: {
			'$lib': path.resolve(__dirname, './src/lib'),
			'$app/state': path.resolve(__dirname, './src/test-mocks/app-state.ts'),
			'$app/navigation': path.resolve(__dirname, './src/test-mocks/app-navigation.ts')
		}
	}
});
