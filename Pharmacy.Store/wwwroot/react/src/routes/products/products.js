import React from 'react';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import SearchDrug from './../../shared/searchDrug/searchDrug';
import Skeleton from '@material-ui/lab/Skeleton';
import { ShowInitErrorAction } from '../../redux/actions/InitErrorAction';
import strings, { enums } from '../../shared/constant';
import srvDrug from './../../service/srvDrug';
import DrugItem from './../../shared/DrugItem/drugIten';
import Heading from './../../shared/heading/heading';

class Products extends React.Component {
    state = {
        loading: true,
        items: []
    }

    async _fetchDrugs() {
        let get = await srvDrug.get({ name: this.props.name });
        console.log(get);
        if (!get.success) {
            this.props.showInitError(this._fetchData.bind(this), get.message);
            return;
        }
        this.setState(p => ({ ...p, loading: false, items: get.result.items }));
    }

    async componentDidMount() {
        console.log('here');
        await this._fetchDrugs();
    }

    render() {
        return (
            <div id='page-products'>
                <Container>
                    <Row id='first-row' className='mb-15'>
                        <Col xs={12} sm={8}>
                            <Row>
                                <Col xs={12}>

                                </Col>
                                <Col xs={12}>
                                    <Row>
                                        {this.state.loading ? [0, 1, 2, 3, 4, 5, 6, 7, 8].map((x, idx) => (<Col xs={12} sm={4}><DrugItem key={idx} loading={true} /></Col>)) :
                                            this.state.items.map((item, idx) => <Col xs={12} sm={4}><DrugItem key={idx} item={item} /></Col>)
                                        }
                                    </Row>

                                </Col>
                            </Row>

                        </Col>
                        <Col sm={4} className='d-none d-sm-flex'>
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

export default connect(null, mapDispatchToProps)(Products);