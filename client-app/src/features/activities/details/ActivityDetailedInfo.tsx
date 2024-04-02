import { observer } from 'mobx-react-lite';
import { Segment, Grid, Icon } from 'semantic-ui-react'
import { Activity } from "../../../app/models/activity";
import { Birds } from "./Birds";
import { format } from 'date-fns';
//import MyCheckboxInput from '../../../app/common/form/MyCheckboxInput';

interface Props {
    activity: Activity,
    birds: typeof Birds,
}

export default observer(function ActivityDetailedInfo({ activity }: Props) {
    const birdsList = Birds.map((bird, index) => {
        return (
            <div key={index}>
                <p><strong>{bird.text}</strong> - {bird.value}</p>
            </div>
        );
    });
    return (
        <Segment.Group>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size='large' color='teal' name='info' />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{activity.description}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='calendar' size='large' color='teal' />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <span>
                            {format(activity.date!, 'dd MMM yyy h:mm aa')}
                        </span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='marker' size='large' color='teal' />
                    </Grid.Column>
                    <Grid.Column width={11}>
                        <span>{activity.venue}, {activity.city}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='marker' size='large' color='teal' />
                    </Grid.Column>
                    <Grid.Column width={11}>
                        {(activity.category.startsWith("Bird") ||
                            activity.category.startsWith("bird")) ?
                            <span>
                                {birdsList}
                            </span> : null
                        }
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
})

