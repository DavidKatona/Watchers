using UnityEngine;

public class PlayBehaviourSound : StateMachineBehaviour
{
    private AudioSource _audioSource;
    public AudioClip audioClip;
    public bool loop = false;
    public float volume = 1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _audioSource = animator.transform.GetComponent<AudioSource>();
        _audioSource.clip = audioClip;
        _audioSource.loop = loop;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float soundPitch = Random.Range(0.8f, 1.2f);
        _audioSource.pitch = soundPitch;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _audioSource.Stop();
        _audioSource.loop = false;
    }
}
