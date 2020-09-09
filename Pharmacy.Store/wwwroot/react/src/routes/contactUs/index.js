import React from 'react';
import strings from '../../shared/constant';
import { Container, Row, Col } from 'react-bootstrap';
// import generalSrv from './../../service/generalSrv';
import Skeleton from '@material-ui/lab/Skeleton';
import whatsappImage from './../../assets/images/whatsapp.png';
import telegramImage from './../../assets/images/telegram.png';
import { ShowInitErrorAction } from '../../redux/actions/InitErrorAction';
import { connect } from 'react-redux';


class ConactUs extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            telegramLink: '',
            whatsappLink: '',
            websiteUrl: 'http://hillavas.com',
            websiteName: 'هیلاوس',
            phoneNumbers: ['96998998', '96998998']
        }
    }

    // async _fetchData() {
    //     let srvRep = await generalSrv.getContactUsInfo();
    //     if (!srvRep.success) {
    //         this.props.showInitError(this._fetchData.bind(this), srvRep.message);
    //         return;
    //     }
    //     this.setState(p => ({ ...p, ...srvRep.result, loading: false }));
    // }

    // async componentDidMount() {
    //     await this._fetchData();
    // }

    render() {
        return (
            <div className="contact-us-page ">
                <Container>
                    <Row>
                        <Col xs={12} sm={12}>
                            <div className='card page-comp w-100'>
                                <Row className='socials'>
                                    <Col xs={6} sm={6} className='d-flex justify-content-center'>
                                        {this.state.loading ? <Skeleton className='rounded' height={40} width={130} variant='rect' /> :
                                            <a className='a-whatsapp' href={this.state.whatsappLink}>
                                                Whats App
                                                <img src={whatsappImage} alt='whatsapp' />
                                            </a>}
                                    </Col>
                                    <Col xs={6} sm={6} className='d-flex justify-content-center'>
                                        {this.state.loading ? <Skeleton className='rounded' height={40} width={130} variant='rect' /> :
                                            <a className='a-telegram' href={this.state.telegramLink}>
                                                Telegram
                                                <img src={telegramImage} alt='telegram' />
                                            </a>}
                                    </Col>
                                </Row>
                                <Row>
                                    <Col xs={6} sm={6} className='website'>
                                        <label className='m-b text-center'>{strings.website}:</label>
                                        {this.state.loading ? <Skeleton variant='text' /> : <a className='text-center' href={this.state.websiteUrl}>{this.state.websiteName}</a>}
                                    </Col>
                                    <Col xs={6} sm={6} className='phone-numbers'>
                                        <label className='m-b text-center'>{strings.phoneNumbers}:</label>
                                        {this.state.loading ? [1, 2].map(x => <Skeleton key={x} variant='text' />) :
                                            this.state.phoneNumbers.map((x, idx) => (<a className='mb-15  text-center' href={`tel:009821${x}`} key={idx}>{x}</a>))}
                                    </Col>
                                </Row>
                            </div>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
    }
}


const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message))
});

export default connect(null, mapDispatchToProps)(ConactUs);