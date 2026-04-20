<script lang="ts">
	import { page } from '$app/state';
	import '../app.css';

	let { children } = $props();

	const navLinks = [
		{ href: '/list', label: 'List' },
		{ href: '/calendar', label: 'Calendar' },
		{ href: '/map', label: 'Map' }
	];

	let navOpen = $state(false);
</script>

<svelte:head>
	<title>FFBC Calendar</title>
</svelte:head>

<header>
	<nav class="navbar navbar-expand-sm navbar-dark">
		<div class="container">
			<a class="navbar-brand" href="/">FFBC Calendar</a>
			<button class="navbar-toggler" type="button" onclick={() => (navOpen = !navOpen)} aria-label="Toggle navigation">
				<span class="navbar-toggler-icon"></span>
			</button>
			<div class="collapse navbar-collapse" class:show={navOpen}>
				<ul class="navbar-nav ms-auto">
					{#each navLinks as link}
						<li class="nav-item">
							<a
								class="nav-link"
								class:active={page.url.pathname === link.href}
								href={link.href}
								onclick={() => (navOpen = false)}
							>
								{link.label}
							</a>
						</li>
					{/each}
				</ul>
			</div>
		</div>
	</nav>
</header>

<main class="container py-4">
	{@render children()}
</main>

<footer class="footer text-center">
	<div class="container">
		&copy; {new Date().getFullYear()} FFBC Calendar &middot;
		<a href="/feed.xml" class="text-muted">RSS Feed</a>
	</div>
</footer>
