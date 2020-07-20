import React from 'react';
import strings from '../../shared/constant';
import { Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';

import Skeleton from '@material-ui/lab/Skeleton';

import { ShowInitErrorAction } from '../../redux/actions/InitErrorAction';

class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
        }
    }

    async _fetchData() {
        // let srvRep = await generalSrv.getContactUsInfo();
        // if (!srvRep.success) {
        //     this.props.showInitError(this._fetchData.bind(this), srvRep.message);
        //     return;
        // }
        //this.setState(p => ({ ...p, ...srvRep.result, loading: false }));
    }

    async componentDidMount() {
        console.log('here');
        await this._fetchData();
    }

    render() {
        return (
            <div id='home-page'>
                
                <Container>
                </Container>
            </div>
        );
    }
}


const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message))
});

export default connect(null, mapDispatchToProps)(Home);