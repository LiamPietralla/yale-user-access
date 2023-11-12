const anonymousRoutes = ['/login', '/health']; 

export default defineNuxtRouteMiddleware((to, from) => {
    // If the route is not the login page and the user is not logged in then redirect to login
    const authenticated = useCookie<boolean>('authenticated');
    if (!anonymousRoutes.includes(to.path) && !(authenticated.value) ) {
        return navigateTo('/login');
    }
})