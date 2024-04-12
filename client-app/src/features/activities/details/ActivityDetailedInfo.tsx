import { observer } from 'mobx-react-lite';
import { Segment, Grid, Icon } from 'semantic-ui-react';
import { Activity } from "../../../app/models/activity";
import { Birds } from "./Birds";
import { format } from 'date-fns';
import { useEffect, useState } from 'react';

interface Props {
    activity: Activity,
    birds: typeof Birds,
}

export default observer(function ActivityDetailedInfo({ activity }: Props) {
    const [checked, setChecked] = useState(true);

    useEffect(() => {
        console.log("Checkbox state changed", checked);
    }, [checked]);

    const birdsList = Birds.map((bird, index) => {
        return (
            <div key={index}>
                {bird.text.startsWith("SWE") ? (
                    <p>
                        <span>
                            {bird.value}
                            <input
                                id={bird.id.toString()}
                                type='checkbox'
                                checked={bird.checked}
                                onChange={() => setChecked(checked => !checked)}
                                onClick={() => bird.checked = !bird.checked}
                            ></input>
                        </span>
                    </p>
                ) : null}
            </div>
        );
    });

    const HEADER_DETAILED_INFO = 'Birds to catch';

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
                        {activity.category.startsWith("bird") ?
                            <>
                                <div><strong>{HEADER_DETAILED_INFO}</strong></div>
                                <span>
                                    {birdsList}
                                </span>
                            </> : null
                        }
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
})