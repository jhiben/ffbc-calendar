export interface Event {
	date: string;
	title: string;
	notes: string | null;
	town: string | null;
	postalCode: string | null;
	country: string | null;
	province: string | null;
	challenge: string | null;
	eventId: string | null;
}

export interface Activity {
	type: string | null;
	distance: string | null;
	elevation: string | null;
	difficulty: string | null;
	time: string | null;
	ravito: string | null;
	price: string | null;
}

export interface EventDetails {
	eventId: string;
	htmlContent: string | null;
	registrationUrl: string | null;
	startLocation: string | null;
	address: string | null;
	mapsUrl: string | null;
	club: string | null;
	notes: string | null;
	website: string | null;
	activities: Activity[];
}

export interface GeoCoord {
	latitude: number;
	longitude: number;
}
