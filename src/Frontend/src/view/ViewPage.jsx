import { useParams } from 'react-router-dom';
import { history } from '../_helpers';
import { UpsertForm } from '../components';

function ViewPage() {
  const id = useParams().id;

  const afterSubmit = () => history.navigate('/');

  return (
    <div className="row justify-content-center">
      <div className="col-12 col-md-8 col-lg-6">
        <UpsertForm id={id} afterSubmit={afterSubmit} />
      </div>
    </div>
  );
}

export { ViewPage };
