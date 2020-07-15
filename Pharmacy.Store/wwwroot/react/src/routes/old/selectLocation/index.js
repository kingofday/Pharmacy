import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'react-bootstrap';
import CustomMap from '../../shared/map';
import { SetLocationAction } from './../../redux/actions/mapAction';
import strings from './../../shared/constant';
import Header from './../../shared/header';
import queryString from 'query-string'

class SelectLocation extends React.Component {
    constructor(props) {
        super(props);
        const values = queryString.parse(this.props.location.search)

        if (values['lng']) this.lng = parseFloat(values['lng'])
        else this.lng = null;
        if (values['lat']) this.lat = parseFloat(values['lat'])
        else this.lat = null;
    }

    _mapChanged(lng, lat) {
        this.lng = lng;
        this.lat = lat;
    }

    _setLocation() {
        this.props.setLocation(this.lng, this.lat);
        this.props.history.goBack();
    }
    render() {
        return (
            <div className="select-location-page with-header">
                <Header hasTitle={true} title={strings.map} goBack={this.props.history.goBack} />
                <CustomMap height='100vh' lng={this.lng} lat={this.lat} onChanged={this._mapChanged.bind(this)} />
                <button className='btn-next' onClick={this._setLocation.bind(this)}>{strings.selectLocation}</button>
            </div>
        );
    }

}

// const mapStateToProps = state => {
//     return { ...state.basketReducer };
// }

const mapDispatchToProps = dispatch => ({
    setLocation: (lat, lng) => dispatch(SetLocationAction(lat, lng))
});

export default connect(null, mapDispatchToProps)(SelectLocation);
