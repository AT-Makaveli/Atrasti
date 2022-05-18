import React, {Component} from 'react';
import {Route} from 'react-router';
import {Layout} from './components/Layout';
import {Home} from './components/Home';

import './custom.css'
import {Register} from './components/auth/Register';
import {Login} from './components/auth/Login';
import AboutUs from "./components/Routes/AboutUs";

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home}/>
                <Route path='/AboutUs' component={AboutUs} />
                <Route path='/register' component={Register}/>
                <Route path='/login' component={Login}/>
            </Layout>
        );
    }
}
