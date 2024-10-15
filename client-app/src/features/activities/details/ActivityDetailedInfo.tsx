import { observer } from 'mobx-react-lite';
import { Segment, Grid, Icon } from 'semantic-ui-react';
import { Activity } from "../../../app/models/activity";
import { Birds } from "./Birds";
import { format } from 'date-fns';
import { useEffect, useState } from 'react';
import axios from 'axios';

interface Props {
    activity: Activity
}

export default observer(function ActivityDetailedInfo({ activity }: Props) {
    const [checked, setChecked] = useState(true);
    const [wikibirds, setWikibirds] = useState([]);

    useEffect(() => {
        console.log("Checkbox state changed", checked);
    }, [checked]);

    useEffect(() => {
        // Fetching bird data
        axios.get(newFunction())
            .then(response => {
                setWikibirds(response.data);
            })
            .catch(error => {
                console.error('Error fetching bird data from url:', error);
            });

        function newFunction(): string {
            return "https://sv.wikipedia.org/wiki/Lista_%C3%B6ver_f%C3%A5gelarter_observerade_i_Sverige_(taxonomisk)";
        }
    }, []);

    const birdsList = Birds.map((bird, index) => {
        return (
            <div key={index}>
                {bird.text.startsWith("SWE") ? (
                    <p>
                        <span>
                            <input
                                id={bird.id.toString()}
                                type='checkbox'
                                checked={bird.checked}
                                onChange={() => setChecked(checked => !checked)}
                                onClick={() => bird.checked = !bird.checked}
                            ></input>
                            {"   "}
                            {bird.value}
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
                        {/* <Icon name='marker' size='large' color='teal' /> */}
                    </Grid.Column>
                    <Grid.Column width={11}>
                        {activity.category.startsWith("bird") ?
                            <>
                                <div><h1>{HEADER_DETAILED_INFO}</h1></div>
                                <span>
                                    {birdsList.sort((a, b) => (a.key ? a.key : 0) > (b.key ? b.key : 0) ? 1 : -1)}
                                </span>
                                <div>{wikibirds}</div>
                            </> : null
                        }
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
})