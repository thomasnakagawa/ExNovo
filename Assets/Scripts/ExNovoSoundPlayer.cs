using UnityEngine;

namespace ExNovo
{
    /// <summary>
    /// Plays the UI sound effects in ExNovo UI. ExNovoInputHandler calls the public methods of this class to play sounds.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class ExNovoSoundPlayer : MonoBehaviour
    {
        [Header("UI sound clips")]
        [SerializeField] private AudioClip Select1Sound = default;
        [SerializeField] private AudioClip Select2Sound = default;
        [SerializeField] private AudioClip Select3Sound = default;
        [SerializeField] private AudioClip ConfirmSound = default;
        [SerializeField] private AudioClip CancelSound = default;
        [SerializeField] private AudioClip ErrorSound = default;
        private AudioSource AudioSource;

        // Use this for initialization
        void Start()
        {
            AudioSource = GetComponent<AudioSource>();
            if (AudioSource == null)
            {
                throw new MissingComponentException("AudioSource required");
            }
        }

        public void PlaySelectSound(int selectNumber)
        {
            switch (selectNumber)
            {
                case 1:
                    PlaySoundClip(Select1Sound);
                    break;
                case 2:
                    PlaySoundClip(Select2Sound);
                    break;
                case 3:
                    PlaySoundClip(Select3Sound);
                    break;
                default:
                    throw new System.NotImplementedException("Select sound not implemented for select number " + selectNumber);
            }
        }

        public void PlayConfirmSound()
        {
            PlaySoundClip(ConfirmSound);
        }

        public void PlayCancelSound()
        {
            PlaySoundClip(CancelSound);
        }

        public void PlayErrorSound()
        {
            PlaySoundClip(ErrorSound);
        }

        private void PlaySoundClip(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }
    }
}
