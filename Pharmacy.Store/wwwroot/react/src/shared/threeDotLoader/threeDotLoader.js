import React from 'react';

export default class ThreeDotLoader extends React.Component {
    render() {
        if (this.props.loading)
            return (
                <span className="comp-three-dot-loader"><span className="dot"></span><span className="dot"></span><span className="dot"></span></span>
            )
        else return null;
    }
}