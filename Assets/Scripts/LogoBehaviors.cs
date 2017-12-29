using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoBehaviors : MonoBehaviour {

    Rigidbody2D rb;
    GameObject gameOver;
    GameObject bounceCount;
    GameObject bounceCountTitle;
    GameObject arrow;
    GameObject dragIns;
    GameObject music;
    GameObject tvBg;
    GameObject filter;

    //Vector2 dir = new Vector2(3.1f, 1.76f);
    Vector2 dir = new Vector2(1, 1);
    float fScale = 50;
    float fScaleBg = -10;
    int count = 0;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        bounceCount = GameObject.FindGameObjectWithTag("BounceCount");
        bounceCountTitle = GameObject.FindGameObjectWithTag("BounceCountTitle");
        arrow = GameObject.FindGameObjectWithTag("Arrow");
        dragIns = GameObject.FindGameObjectWithTag("DragInstruction");
        music = GameObject.FindGameObjectWithTag("Music");
        tvBg = GameObject.FindGameObjectWithTag("TVBg");
        filter = GameObject.FindGameObjectWithTag("Filter");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.velocity = Vector2.zero;
        tvBg.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector2 targetDir;
        switch (collision.name)
        {
            case "Left":
            case "Right":
                targetDir = new Vector2(dir.x * -1, dir.y);
                break;
            case "Top":
            case "Bottom":
                targetDir = new Vector2(dir.x, dir.y * -1);
                break;
            default:
                gameOver.GetComponent<Text>().enabled = true;
                music.GetComponent<AudioSource>().Stop();
                StopCoroutine("AnimateLogoColor");
                StopCoroutine("AnimateFilterColor");
                return;
        }
        rb.AddForce(targetDir * fScale);
        tvBg.GetComponent<Rigidbody2D>().AddForce(targetDir * fScaleBg);
        dir = targetDir;
        bounceCount.GetComponent<Text>().text = (++count).ToString();
    }

    private void OnMouseDrag()
    {
        if (rb.velocity == Vector2.zero)
        {
            arrow.GetComponent<SpriteRenderer>().enabled = true;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Rad2Deg * Mathf.Atan(mousePos.y / mousePos.x) + (mousePos.x > 0 ? 180f : 0f);
            GameObject.Find("Arrow").transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnMouseUp()
    {
        if (rb.velocity == Vector2.zero)
        {
            float angle = arrow.transform.eulerAngles.z * Mathf.Deg2Rad;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            rb.AddForce(dir * fScale);
            tvBg.GetComponent<Rigidbody2D>().AddForce(dir * fScaleBg);
            arrow.GetComponent<SpriteRenderer>().enabled = false;
            dragIns.GetComponent<Text>().enabled = false;
            bounceCount.GetComponent<Text>().enabled = true;
            bounceCountTitle.GetComponent<Text>().enabled = true;
            music.GetComponent<AudioSource>().Play();
            StartCoroutine("ShowTVBg");
            StartCoroutine("AnimateLogoColor");
            StartCoroutine("AnimateFilterColor");
        }
    }

    IEnumerator ShowTVBg()
    {
        yield return new WaitForSeconds(23.5f);
        tvBg.GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator AnimateLogoColor()
    {
        yield return new WaitForSeconds(23.5f);
        while (true)
        {
            var sr = GetComponent<SpriteRenderer>();
            sr.color = new Color(
                (sr.color.r + Random.Range(0, 0.5f)) % 1,
                (sr.color.g + Random.Range(0, 0.5f)) % 1,
                (sr.color.b + Random.Range(0, 0.5f)) % 1,
                sr.color.a
            );
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator AnimateFilterColor()
    {
        yield return new WaitForSeconds(138.5f);
        var filterSr = filter.GetComponent<SpriteRenderer>();
        filterSr.enabled = true;
        while (true)
        {
            filterSr.color = new Color(
                (filterSr.color.r + Random.Range(0, 0.5f)) % 1,
                (filterSr.color.g + Random.Range(0, 0.5f)) % 1,
                (filterSr.color.b + Random.Range(0, 0.5f)) % 1,
                filterSr.color.a
            );
            yield return new WaitForSeconds(0.25f);
        }
    }
}
