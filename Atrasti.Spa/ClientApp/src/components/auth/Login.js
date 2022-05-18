import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { LoginActions } from '../Constants';
import authService from "../../services/AuthenticationService";

export class Login extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isAuthenticated: false,
            emailValue: '',
            passwordValue: ''
        };

        this.onEmailChange = this.onEmailChange.bind(this);
        this.onPasswordChange = this.onPasswordChange.bind(this);
        this.afterSubmission = this.afterSubmission.bind(this);
    }

    render() {
        return (
            <div style={{ backgroundSize: 'cover', backgroundPosition: 'center', backgroundImage: "url(https://images.unsplash.com/photo-1547932087-59a8f2be576e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1950&q=80)" }}>
                <div id="body-content">
                    <Container>

                        <div className="d-flex justify-content-center vh-100">
                            <div id="login-card" className="bg-light">
                                <div className="d-flex justify-content-center">

                                    <div className="w-85" id="login-form-wrapper">
                                        <br />

                                        <form className="mb-2" method="post" action={LoginActions.Login} onSubmit={this.afterSubmission}>

                                            <div className="input-group mb-2">
                                                <input type="email"
                                                    className="form-control"
                                                    name="Email" value={this.state.emailValue} onChange={this.onEmailChange} placeholder="Email" required autoComplete="email" />

                                                <div className="input-group-append">
                                                    <span className="input-group-text text-white" style={{ backgroundColor: '#FFF' }}>
                                                        <i className="fa fa-user" style={{ color: '#5a6268' }}></i>
                                                    </span>
                                                </div>
                                            </div>

                                            <div className="input-group mb-2">
                                                <input type="password" placeholder="Password"
                                                    className="form-control" id="password" name="Password"
                                                    required
                                                    autoComplete="current-password" value={this.state.passwordValue} onChange={this.onPasswordChange} />
                                                <div className="input-group-append">
                                                    <span className="input-group-text text-white" style={{ backgroundColor: '#FFF' }}>
                                                        <i className="fa fa-lock" style={{ color: '#5a6268' }}></i>
                                                    </span>
                                                </div>
                                            </div>

                                            <input type="submit" value="Login" className="form-control btn btn-secondary"></input>

                                            <div className="input-group mb-2">
                                                <div className="form-check">
                                                    <input className="form-check-input" id="Remember" type="checkbox" value="true" name="RememberMe" />

                                                    <label className="form-check-label" htmlFor="remember">Remember Me</label>
                                                </div>
                                            </div>
                                        </form>

                                        <a className="w-100 text-center d-block text-dark">Reset Password</a>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </Container>
                </div>
            </div>
        );
    }

    onEmailChange(event) {
        const emailValue = event.target.value;

        this.setState({
            emailValue: emailValue
        })
    }

    onPasswordChange(event) {
        const passwordValue = event.target.value;

        this.setState({
            passwordValue: passwordValue
        })
    }

    async afterSubmission(event) {
        event.preventDefault();
        
        const email = this.state.emailValue;
        const password = this.state.passwordValue;
        
        await authService.login(email, password);
    }

}