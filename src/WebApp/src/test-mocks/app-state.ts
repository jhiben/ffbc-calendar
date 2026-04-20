// Plain object mock for $app/state - no $state rune needed since
// page properties are set before each render call in tests.
export const page: Record<string, any> = {
	url: new URL('http://localhost/'),
	params: {},
	route: { id: '/' },
	status: 200,
	error: null,
	data: {},
	form: null,
	state: {}
};
