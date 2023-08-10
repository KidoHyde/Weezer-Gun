using System;
using UnityEngine;
// Token: 0x02000257 RID: 599

namespace WeezerGunUtils
{


    // Token: 0x02000258 RID: 600
    public class WeezerAnimationReceiver : MonoBehaviour
    {
        // Token: 0x06000D86 RID: 3462 RVA: 0x0007C250 File Offset: 0x0007A450
        private void Start()
        {
            this.rev = base.GetComponentInParent<Weezerrev>();
        }

        // Token: 0x06000D87 RID: 3463 RVA: 0x0007C25E File Offset: 0x0007A45E
        public void ReadyGun()
        {
            this.rev.ReadyGun();
        }

        // Token: 0x06000D88 RID: 3464 RVA: 0x0007C26B File Offset: 0x0007A46B
        public void Click()
        {
            if (this.click)
            {
                Instantiate<GameObject>(this.click);
            }
            this.rev.cylinder.DoTurn();
            this.rev.Click();
        }

        // Token: 0x04001470 RID: 5232
        private Weezerrev rev;

        // Token: 0x04001471 RID: 5233
        public GameObject click;
    }


}
