import Config from "./config";
import { handleResponse } from "./ResponseService";

class AuthenticationService {

    constructor() {
        if (localStorage.getItem('currentUser') !== null) {
            this.currentUserSubject = JSON.parse(localStorage.getItem('currentUser'));
        } else {
            this.currentUserSubject = null;
        }
    }

    async login(email, password) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Email: email, Password: password })
        };

        return fetch(`auth/login`, requestOptions)
            .then(handleResponse)
            .then(user => {
                console.log(user);
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject = user;

                return user;
            });
    }

    logout() {
        this.currentUserSubject = null;
        localStorage.setItem('currentUser', null);
    }
    
    isAuthenticated() {
        return this.currentUserSubject !== null;
    }
}

const authService = new AuthenticationService();

export default authService;