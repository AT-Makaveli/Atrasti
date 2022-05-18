import React, {Component} from 'react';
import './css/register.css';

export class Register extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <div id="body-content" className="min-vh-100">
                    <div className="row">
                        <div className="col-md-5">
                        </div>
                        <div className="col-md-7 bg-light">
                            <div className="container">
                                <div className="min-vh-100" id="login-form-wrapper">
                                    <div className="text-center">
                                        <h1 className="display-5">
                                            Join Atrasti
                                        </h1>
                                        <p>Already have an account? Sign in.</p>
                                    </div>
                                    <br/>
                                    <div asp-validation-summary="All" className="text-danger"/>
                                    <form method="POST">
                                        <div className="form-group">
                                            <label htmlFor="Company">Company</label>
                                            <div className="input-group">
                                                <input id="Company" type="text" asp-for="Company"
                                                       className="form-control"
                                                       name="Company" value="" required
                                                       autoComplete="Company"
                                                       style={{borderRight: 'none'}}/>
                                                <div className="input-group-append">
                           <span className="input-group-text text-white"
                                 style={{backgroundColor: '#FFF'}}>
                           <i className="fa fa-building" style={{color: '#5a6268'}}/>
                           </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-6">
                                                <div className="form-group">
                                                    <label htmlFor="FirstName">First name</label>
                                                    <div className="input-group">
                                                        <input id="FirstName" type="text" asp-for="FirstName"
                                                               className="form-control"
                                                               name="FirstName" value=""
                                                               required autoComplete="FirstName"
                                                               style={{borderRight: 'none'}}/>
                                                        <div className="input-group-append">
                                 <span className="input-group-text text-white"
                                       style={{backgroundColor: '#FFF'}}>
                                 <i className="fa fa-user" style={{color: '#5a6268'}}/>
                                 </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-6">
                                                <div className="form-group">
                                                    <label htmlFor="LastName">Last name</label>
                                                    <div className="input-group">
                                                        <input id="LastName" type="text" asp-for="LastName"
                                                               className="form-control"
                                                               name="LastName" value=""
                                                               required autoComplete="LastName"
                                                               style={{borderRight: 'none'}}/>
                                                        <div className="input-group-append">
                                 <span className="input-group-text text-white"
                                       style={{backgroundColor: '#FFF'}}>
                                 <i className="fa fa-user-o" style={{color: '#5a6268'}}/>
                                 </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="Email">Email</label>
                                            <div className="input-group">
                                                <input id="Email" type="text" asp-for="Email"
                                                       className="form-control"
                                                       name="Email" value="" required
                                                       autoComplete="Email"
                                                       style={{borderRight: 'none'}}/>
                                                <div className="input-group-append">
                           <span className="input-group-text text-white"
                                 style={{backgroundColor: '#FFF'}}>
                           <i className="fa fa-at" style={{color: '#5a6268'}}/>
                           </span>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="Password">Password</label>
                                                <div className="input-group">
                                                    <input id="Password" type="password" asp-for="Password" value=""
                                                           className="form-control" name="Password"
                                                           required
                                                           style={{borderRight: 'none'}}/>
                                                    <div className="input-group-append">
                              <span className="input-group-text text-white" style={{backgroundColor: '#FFF'}}>
                              <i className="fa fa-key" style={{color: '#5a6268'}}/>
                              </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="ConfirmPassword">Confirm password</label>
                                                <div className="input-group">
                                                    <input id="ConfirmPassword" asp-for="ConfirmPassword"
                                                           type="password"
                                                           className="form-control"
                                                           name="ConfirmPassword" required style={{borderRight: 'none'}}/>
                                                    <div className="input-group-append">
                              <span className="input-group-text text-white" style={{backgroundColor: '#FFF'}}>
                              <i className="fa fa-check" style={{color: '#5a6268'}}/>
                              </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <ul className="composition">
                                                    <li id="pwd_rule_composition_1" className="nok">A-Z</li>
                                                    <li id="pwd_rule_composition_2" className="nok">a-z</li>
                                                    <li id="pwd_rule_composition_3" className="nok">0-9</li>
                                                    <li id="pwd_rule_composition_4" className="nok">!-#</li>
                                                    <li id="pwd_rule_composition_5" className="nok">≥8</li>
                                                </ul>
                                            </div>
                                            <div className="form-group">
                                                <input type="submit" value="Register"
                                                       className="form-control btn btn-blue"/>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

}