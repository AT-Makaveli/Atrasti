import React, {Component} from "react";

export default class AboutUs extends Component {
    render() {
        return (
            <div>
                <section id="page-top">
                    <div id="header-content" className="p-4">

                        <div id="hero" className="text-center pb-18" style={{
                            borderRadius: '0.5rem',
                            background: 'url(https://atrasti.com/img/hero/basketball.jpg) center no-repeat',
                            backgroundSize: 'cover'
                        }}>
                            <div id="hero-header">
                                <div className="jumbotron bg-transparent">
                                    <h2 className="text-white pt-5">Welcome to Atrasti!</h2>
                                    <h5 className="text-white p-3">
                                        Atrasti is a platform for value generating searches, we make it easy for companies to find
                                        and
                                        connect with each other.
                                    </h5>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <section id="page-content" className="pt-5 pb-5 shadow">


                    <div className="big-container">
                        <div className="row p-4">
                            <div className="text-white">
                                <h1>A better opportunity for everyone</h1>

                                <p className="lead">We want to create a digital meeting place for companies to find and
                                    develop relationships with other companies.</p>

                                <h2>Our story</h2>

                                <p className="lead">The way to search, find, and integrate with businesses is changing
                                    in an increasingly digital world. At Atrasti, we work for our users to be able to
                                    find what they are looking for and for them to feel comfortable that the profile
                                    hits they receive are of high quality, with high quality we mean companies which
                                    hold a good reputation among others and are good to start relationships and
                                    collaborate with. We exist because our founder lacked the functions, we have created
                                    during his time working in the Supply Chain area.</p>

                                <h2>Is Atrasti really free? It is and will always be.</h2>

                                <p className="lead">We exist to inspire and encourage development in the business world,
                                    for this development we have created Atrasti and our contribution to accelerating
                                    this development is to be free. So don't wait and sign up for free.</p>

                                <h2>Join the Atrasti family. From anywhere.</h2>

                                <p className="lead">
                                    Help us creating the digital meeting place of tomorrow for businesses all around the
                                    world.
                                    Unfortunately, we don’t have any positions available at the moment. That said, we’re
                                    always happy to hear from talented people, so if you think you’d be a great new
                                    addition to our team, please get in touch by emailing careers@atrasti.com
                                </p>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        );
    }
}