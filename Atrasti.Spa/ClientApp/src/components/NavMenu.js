import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm shadow navbar-toggleable-sm box-shadow bg-dark">
                    <NavbarBrand tag={Link} to="/"><img src="/img/atrasti_logo.png" /></NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav mr-auto">
                            <NavItem>
                                <NavLink tag={Link} className="text-white hover-underline" to="/AboutUs">About us</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-white hover-underline" to="/">Contact</NavLink>
                            </NavItem>
                        </ul>
                        <ul className="navbar-nav ml-auto">
                            <NavItem>
                                <NavLink tag={Link} className="text-blue hover-underline" to="/login">Login</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-blue btn btn-outline-blue" to="/register">Register</NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        );
    }
}
